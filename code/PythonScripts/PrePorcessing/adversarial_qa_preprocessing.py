# -*- coding: utf-8 -*-
"""adversarial_qa-preprocessing.ipynb

Automatically generated by Colaboratory.

Original file is located at
    https://colab.research.google.com/drive/1yQh-Psga-cakQ-ALGHLJGx91ycEbioBR

# Install Packages
"""

!pip install transformers
!pip install datasets

import datasets
import transformers
import datetime
import os
from datasets import load_dataset
from datasets import Features, Value, Sequence
from tqdm import tqdm
from string import punctuation
import spacy
import numpy as np
from pathlib import Path
import torch
import locale
# spacy.cli.download("en_core_web_lg")
nlp = spacy.load('en_core_web_sm')

"""# Load Dataset"""

from datasets import load_dataset

dataset = load_dataset('adversarial_qa', 'adversarialQA')

dataset

questions_list = []
answers_list = []
passsages_list =[]


import re
pattern = r'\b(who|what|when|where|why|how|which)'
# mylist1.index(max(mylist1))
for example in dataset['train']:
  if(len(example['answers']['text'])==0):
    continue
  q=example['question']
  if re.match(pattern,q.lower()):
    questions_list.append(q)
    answers_list.append(example['answers']['text'][0])
    passsages_list.append(example['context'])

for example in dataset['validation']:
  if(len(example['answers']['text'])==0):
    continue
  q=example['question']
  if re.match(pattern,q.lower()):
    questions_list.append(q)
    answers_list.append(example['answers']['text'][0])
    passsages_list.append(example['context'])

print(questions_list[0])
print(passsages_list[0])
print(answers_list[0])

print(len(questions_list))
print(len(passsages_list))
print(len(answers_list))

p = set()
for ps in passsages_list:
  p.add(len(ps))

import nltk
nltk.download('punkt')

# Initialize a dictionary to store the counts
# List of interrogative words
interrogative_words = ["who", "what", "where", "when", "why", "how","which"]
word_counts = {word: 0 for word in interrogative_words}
# Iterate over each question
for question in questions_list:
    # Tokenize the question into words
    words = nltk.word_tokenize(question)

    # Convert words to lowercase
    words = [word.lower() for word in words]

    # Count the interrogative words
    for word in words:
        if word in interrogative_words:
            word_counts[word] += 1

# Print the word counts
for word, count in word_counts.items():
    print(word + ": " + str(count))

answers = list()
questions = list()
passages = list()

word_counts = {word: 0 for word in interrogative_words}

word_counts['what'] +=2000
word_counts['who'] +=2000
word_counts['when'] +=1000
word_counts['how'] +=2000
word_counts['which'] +=2000
word_counts['where'] +=1495
word_counts['why'] +=902

for idx,question in enumerate(questions_list):
    words = nltk.word_tokenize(question)

    # Convert words to lowercase
    words = [word.lower() for word in words]

    # Count the interrogative words
    for word in words:
      if word in interrogative_words and word_counts[word]!=0:
        answers.append(answers_list[idx])
        questions.append(question)
        passages.append(passsages_list[idx])
        word_counts[word] -=1

print(len(questions))
print(len(answers))
print(len(passages))

"""# Tokenize Dataset (passage - answer) pair"""

tokenizer = transformers.BertTokenizer.from_pretrained('bert-base-uncased')

# iterates through pairs of passages and answers and calls tokenizer.encode_plus() on each pair.
# tokenizes a piece of text(passages, answers) and returns a dictionary containing various pieces of information about the tokens.
'''
  the resulting dictionary contains two keys: input_ids and attention_mask.
  input_ids is a tensor that contains the token IDs of the input sequence,
  and attention_mask is a tensor that contains a binary mask indicating which tokens are padding and which ones are not.
'''


def tokenize_PassageAnswer_pairs(passages, answers):
  input_ids = []
  attention_masks = []

  for passage, answer in tqdm(zip(passages, answers)):
      encoded_dict = tokenizer.encode_plus(
                          passage,
                          answer,
                          add_special_tokens = True,
                          return_token_type_ids = False,
                          max_length = 512,
                          truncation=True,
                          truncation_strategy='only_first',
                          padding="max_length",
                          return_attention_mask = True,
                          return_tensors = 'pt'
                    )
      input_ids.append(encoded_dict['input_ids'][:,:512])
      attention_masks.append(encoded_dict['attention_mask'])
  return input_ids,attention_masks

input_ids,attention_masks=tokenize_PassageAnswer_pairs(passages,answers)

"""# Extract Entity Types"""

# dictionary to map each entity to its tensor
entity_type_dict = {'PERSON': [1, 0, 0, 0, 0], 'CARDINAL': [0, 1, 0, 0, 0], 'DATE': [0, 0, 1, 0, 0], 'ORG': [0, 0, 0, 1, 0], 'GPE': [0, 0, 0, 0, 1], 'None': [0, 0, 0, 0, 0]}
interrogative_words = {'who':0, 'what':1, 'when':2, 'where':3, 'why':4, 'how':5,'which':6,'other':7}

entity_type_embeddings = list()
for answer in tqdm(answers):
    doc = nlp(answer)
    if doc.ents:
      entity_types = set([token.ent_type_ if token.ent_type_ != "" else 'None' for token in doc])
    else:
      entity_types = set(['None'])
    entities = set()
    entity_list = list()
    for ent in entity_types:
      if ent in entity_type_dict:
        entities.add(ent)
      else :
        entities.add('None')

    for ent in entities:
      try:
        entity = (entity_type_dict[ent])
      except:
        entity = (entity_type_dict['None'])
      entity_list.append(entity)

    enity_length = len(entity_list)
    if enity_length < 6:
      for i in range(6-enity_length):
        entity_list.append(entity_type_dict['None'])

    entity_type_embeddings.append(entity_list)

"""# Extract Interrogative Word"""

def question_InterrogativeWord_Extraction(questions):
  found_interrogative_words = []
  for q in tqdm(questions):
    words = tokenizer.tokenize(q)
    for word in words:
      found = False
      if word.lower() in interrogative_words:
          found=True
          found_interrogative_words.append(interrogative_words[word.lower()])
          break
    if found==False:
          found_interrogative_words.append(interrogative_words['other'])

  return found_interrogative_words

found_interrogative_words=question_InterrogativeWord_Extraction(questions)

"""# Save Dataset"""

train_ds_file = Path(r'/content/drive/MyDrive/Datasets/train_adversarial_qa_dataset.pt')

input_ids = torch.cat(input_ids, dim=0)
attention_masks = torch.cat(attention_masks, dim=0)
entity_type_embeddings = torch.tensor(entity_type_embeddings)
interrogative_words_labels=torch.tensor(found_interrogative_words,dtype=torch.int64)

input_ids.shape

from google.colab import drive
drive.mount('/content/drive')

dataset_train = torch.utils.data.TensorDataset(input_ids, attention_masks, entity_type_embeddings,interrogative_words_labels)
torch.save(dataset_train,train_ds_file)