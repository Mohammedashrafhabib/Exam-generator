using DocumentFormat.OpenXml.InkML;
using EGService.Core.IServices;
using EGService.Core.Models.ViewModels.ExamGenrator;
using Framework.Core.Common;
using Microsoft.Extensions.Configuration;
using NPOI.OpenXmlFormats.Dml;
using Python.Runtime;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;

namespace EGService.Business.Services
{
    public class ExamGenrator : Base.BaseService, IExamGenrator
    {
        private IConfiguration _iConfiguration;

        string IntrogativeWord = "";
        string AnswerExtractionModelPath = "";
        string AnswerExtractionToken_Path = "";
        string BoolModelPath = "";

        string QuestionGenration = "";
        string QuestionGenrationToken_Path = "";
        string S2V = "";
        int s2vConst = 10;
        private readonly static PyModule scope = Py.CreateScope();
        ILoggerService _logger;
        public ExamGenrator(IConfiguration IConfiguration, ILoggerService logger)
        {
            try { 
            _iConfiguration = IConfiguration;
            _logger = logger;
            IntrogativeWord = _iConfiguration.GetSection("CommonSettings").GetSection("IntrogativeWord").Value;
            AnswerExtractionModelPath = _iConfiguration.GetSection("CommonSettings").GetSection("AnswerExtractionModelPath").Value;
                BoolModelPath = _iConfiguration.GetSection("CommonSettings").GetSection("BoolModelPath").Value;
                AnswerExtractionToken_Path = _iConfiguration.GetSection("CommonSettings").GetSection("AnswerExtractionToken_Path").Value;
            QuestionGenration = _iConfiguration.GetSection("CommonSettings").GetSection("QuestionGenration").Value;
            QuestionGenrationToken_Path = _iConfiguration.GetSection("CommonSettings").GetSection("QuestionGenrationToken_Path").Value;
            S2V = _iConfiguration.GetSection("CommonSettings").GetSection("S2V").Value;
            s2vConst = int.Parse(_iConfiguration.GetSection("CommonSettings").GetSection("s2vConst").Value);

            if (!PythonEngine.IsInitialized)
            {
                Python.Runtime.Runtime.PythonDLL = _iConfiguration.GetSection("CommonSettings").GetSection("PythonDLL").Value;

                PythonEngine.Initialize();
                PythonEngine.BeginAllowThreads();


                using (Py.GIL())
                {

                    scope.Set("MODEL_PATH", AnswerExtractionModelPath);
                    scope.Set("MODEL_PATHBool", BoolModelPath);

                        scope.Set("Token_Path", AnswerExtractionToken_Path);

                    scope.Set("model_path", IntrogativeWord);
                    scope.Set("MODEL_PATH2", QuestionGenration);
                    scope.Set("Token_Path2", QuestionGenrationToken_Path);
                    scope.Set("S2VPath", S2V);
                   
                        scope.Exec(@"
import transformers
import torch
import spacy
import tensorflow
from transformers import AutoTokenizer, TFT5ForConditionalGeneration
nlp = spacy.load('en_core_web_lg')

'''# Enable Cuda Device'''

device = torch.device('cuda:0' if torch.cuda.is_available()
                      else 'cpu')

'''# FFN'''

class FFN(torch.nn.Module):
    def __init__(self, hidden_size, num_classes):
        super().__init__()
        self.fc1 = torch.nn.Linear(hidden_size, num_classes).to(device)

    def forward(self, x):
        out = self.fc1(x)
        return out

'''#Loading Saved Model'''

model = transformers.BertForSequenceClassification.from_pretrained('bert-base-uncased', num_labels=8).to(device)


checkpoint = torch.load(model_path,map_location=torch.device('cpu'))

model.load_state_dict(checkpoint['model_state_dict'])

ffn = FFN(hidden_size=773, num_classes=8)
ffn.load_state_dict(checkpoint['ffn_state_dict'])
tokenizer = transformers.BertTokenizer.from_pretrained('bert-base-uncased')


####AE


task_prefixAE = 'extract answers: '
encoder_max_lenAE = 1350
decoder_max_lenAE = 128
modelAE=TFT5ForConditionalGeneration.from_pretrained(MODEL_PATH,local_files_only=True)
model_nameAE = 't5-small'
tokenizerAE = AutoTokenizer.from_pretrained(Token_Path,local_files_only=True)

#####QG

'''# Hyperparameters'''

task_prefixQG = 'generate question using question word: '

encoder_max_lenQG = 250
decoder_max_lenQG = 70

model_nameQG = 't5-base'

'''# Loading Saved Model'''

modelQG=TFT5ForConditionalGeneration.from_pretrained(model_nameQG)
modelQG.load_weights(MODEL_PATH2)

tokenizerQG = AutoTokenizer.from_pretrained(Token_Path,local_files_only=True)


#####Distractors

from sense2vec import Sense2Vec
import spacy
import re
from word2number import w2n
from collections import OrderedDict
from nltk.stem import WordNetLemmatizer

#spacy.cli.download('en_core_web_lg')
nlpDis = spacy.load('en_core_web_lg')
s2v = nlpDis.add_pipe('sense2vec')
s2v.from_disk(S2VPath)
def common(s0, s1):
  s0 = s0.lower()
  s1 = s1.lower()
  s0List = s0.split(' ')
  s1List = s1.split(' ')
  return len(list(set(s0List)&set(s1List)))
def sense2vec_get_words(context,s2vNo):
  doc=nlpDis(context)
  output2 = []
  for ent in doc.ents:
    try:
      output = []

      # print(ent.text, ent.end_char, ent.label_)
      most_similar=ent._.s2v_most_similar(s2vNo)
      for each_word in most_similar:
        if each_word[0][1]==ent.label_:
          append_word = each_word[0][0]

          new_append_word = re.sub(r'[^\w]', ' ', append_word.lower())
          new_word = re.sub(r'[^\w]', ' ', ent.text.lower())
          new_word2=''
          if ent.label_=='CARDINAL':
            new_word2=str(w2n.word_to_num(new_word))
          if new_append_word not in new_word and new_word not in new_append_word and common(new_word,new_append_word)==0 and common(new_word2,new_append_word)==0 :
              output.append(append_word.title())
      #print( list(OrderedDict.fromkeys(output)))
      output2.append( [ent.text,list(OrderedDict.fromkeys(output))])
      #print(output2)
    except:
      continue
  return output2
#############SENT_TOKENIZER
import nltk

nltk.download('wordnet')
nltk.download('punkt')
def split_passage_to_sentences(passage):
    sentences = nltk.sent_tokenize(passage)

    return sentences

####BoolQG


task_prefixBool = 'generate boolean question: '
encoder_max_lenBool = 500
decoder_max_lenBool = 70
modelBool=TFT5ForConditionalGeneration.from_pretrained(MODEL_PATHBool,local_files_only=True)
model_nameBool = 't5-small'
tokenizerBool = AutoTokenizer.from_pretrained(Token_Path,local_files_only=True)
number_of_questions = 3
def beam_search_decoding (inp_ids,attn_mask):
  beam_output = modelBool.generate(input_ids=inp_ids,
                                 attention_mask=attn_mask,
                                 max_length=decoder_max_lenBool,
                               num_beams=10,
                               num_return_sequences=number_of_questions,
                               no_repeat_ngram_size=2,
                               early_stopping=True
                               )
  Questions = Questions = tokenizer.batch_decode(beam_output, skip_special_tokens=True)
  return [Question.strip().capitalize() for Question in Questions]


def topkp_decoding (inp_ids,attn_mask):
  topkp_output = modelBool.generate(input_ids=inp_ids,
                                 attention_mask=attn_mask,
                                 max_length=decoder_max_lenBool,
                               do_sample=True,
                               top_k=40,
                               top_p=0.80,
                               num_return_sequences=number_of_questions,
                                no_repeat_ngram_size=2,
                                early_stopping=True
                               )
  Questions = tokenizerBool.batch_decode(topkp_output, skip_special_tokens=True)
  return [Question.strip().capitalize() for Question in Questions]

");
                    


                }
            }
        }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex);
                        _logger.LogError(ex.StackTrace);
                        _logger.LogError(ex.Message);

                        throw;
                    }
}



        public IList<string> AnswerExtractionModel(string Context)
        {

            // create a Python scope

            try
            {


                scope.Set("context", Context);
            scope.Exec(@"

input_textAE = task_prefixAE + 'context: ' + context
encoded_queryAE = tokenizerAE(input_textAE, return_tensors='tf', pad_to_max_length=True, truncation=True, max_length=encoder_max_lenAE)
input_idsAE = encoded_queryAE['input_ids']
attention_maskAE = encoded_queryAE['attention_mask']
generated_answersAE = modelAE.generate(input_idsAE, attention_mask=attention_maskAE, max_length=decoder_max_lenAE, top_p=0.95, top_k=50, repetition_penalty=float(2))
decoded_answersAE = tokenizerAE.decode(generated_answersAE.numpy()[0], skip_special_tokens=True)

");
            string answer = scope.Get<string>("decoded_answersAE");
            var answers = answer.Split(',');
            return answers.ToList();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                _logger.LogError(ex.StackTrace);
                _logger.LogError(ex.Message);

                throw;
            }
        }


        public IList<string> BoolQGModel(string Context)
        {

            // create a Python scope

            try
            {


                scope.Set("context", Context);
                scope.Exec(@"
input_text_true = task_prefixBool + 'True' + ' ' + 'context: ' + context

encoded_queryBool = tokenizerBool(input_text_true, return_tensors='tf', pad_to_max_length=True, truncation=True, max_length=encoder_max_lenBool)
input_idsBool= encoded_queryBool['input_ids']
attention_maskBool = encoded_queryBool['attention_mask']

output_beam = beam_search_decoding(input_idsBool, attention_maskBool)



output_topkp = topkp_decoding(input_idsBool, attention_maskBool)

decoded_questionsBool = set(output_beam + output_topkp)

");
                var decoded_questionsBool = scope.Get<string[]>("decoded_questionsBool");
              
                return decoded_questionsBool.Where(e=>!e.Contains("unused")).ToList();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                _logger.LogError(ex.StackTrace);
                _logger.LogError(ex.Message);

                throw;
            }
        }



        public string IntrogativeWordModel(string Context, string answer)
        {

            // create a Python scope


            scope.Set("passage", Context);
            scope.Set("answer", answer);
            try
            {
                scope.Exec(@"
'''# Passage Answer Encoding'''


encoded_input  = tokenizer.encode_plus(
                      passage,
                      answer,
                      max_length = 512,
                      truncation=True,
                      truncation_strategy='only_first',
                      padding='max_length',
                      return_attention_mask = True,
                      return_tensors = 'pt'
                    )
input_ids ,attention_mask = encoded_input['input_ids'] , encoded_input['attention_mask']

'''# Named Entity For Answer'''

entity_type_dict = {'PERSON': [1, 0, 0, 0, 0], 'CARDINAL': [0, 1, 0, 0, 0], 'DATE': [0, 0, 1, 0, 0], 'ORG': [0, 0, 0, 1, 0], 'GPE': [0, 0, 0, 0, 1], 'None': [0, 0, 0, 0, 0]}
entity_type_embeddings = list()
doc = nlp(answer)
if doc.ents:
  entity_types = set([token.ent_type_ if token.ent_type_ != '' else 'None' for token in doc])
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

for i in range(6- len(entity_list)):
  entity_list.append(entity_type_dict['None'])

entity_type_embeddings.append(entity_list)
named_entity_detected = True
value = torch.tensor([0, 0, 0, 0, 0])
if torch.equal(torch.tensor(entity_type_embeddings[0][0]),value):
  named_entity_detected = False
named_entity_detected

'''# Evaluation'''

# input_ids = torch.cat(input_ids, dim=0)
# attention_masks = torch.cat(attention_masks, dim=0)
entity_type_embeddings = torch.tensor(entity_type_embeddings)

mapping_vector = { 0 : 'who', 1 : 'what', 2 : 'when', 3 : 'where', 4 : 'why', 5 : 'how',6 : 'which',7 : 'other'}
if named_entity_detected:
  model.eval()

  with torch.no_grad():
    outputs = model(input_ids = input_ids, attention_mask = attention_mask, output_hidden_states = True)

  cls_tensor = outputs.hidden_states[-1]
  cls_tensor = cls_tensor[:,:6,:]
  ner_tensor = entity_type_embeddings

  # Concatenate the tensors along the first dimension
  concat_tensor = torch.cat((cls_tensor, ner_tensor),dim=2).to(device)

  logits = ffn(concat_tensor)
  logits=(torch.max(logits,dim=1).values)

  prediction = torch.argmax(logits, dim=1)
  print(prediction)
  prediction = mapping_vector[int(prediction)]
else:
  prediction = mapping_vector[int(1)]

");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                _logger.LogError(ex.StackTrace);
                _logger.LogError(ex.Message);

                throw;
            }
            dynamic prediction = scope.Get("prediction");
            return prediction;


        }


        public IList<string> QuestionGenrationModel(string Context, IList<string> Answers, IList<string> IntrogativeWords)
        {
            try
            {

                string[] questions;

            // create a Python scope


            //var passage = "Software quality assurance(SQA) is the ongoing process that ensures the software product meets and complies with the organization's established and standardized quality specifications its published in 2001";
            //var answer = "2001";
            //var question_word = "what";
            scope.Set("passage", Context);
            scope.Set("answers", Answers.ToArray());
            scope.Set("question_words", IntrogativeWords.ToArray());
            scope.Exec(@"



inputsQG = tokenizerQG([task_prefixQG + question_word + ' ' + 'answer:' + ' '+ answer + ' ' + 'context: ' + passage for answer, question_word in zip(answers, question_words)], return_tensors='tf', padding=True, truncation=True, max_length=encoder_max_lenQG)

generated_questionsQG = modelQG.generate(inputsQG['input_ids'], attention_mask = inputsQG['attention_mask'], max_length = decoder_max_lenQG, top_p = 0.95, top_k = 50, repetition_penalty = float(2))
decoded_questions = tokenizerQG.batch_decode(generated_questionsQG, skip_special_tokens = True)
");
            questions = scope.Get<string[]>("decoded_questions");



            return questions.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                _logger.LogError(ex.StackTrace);
                _logger.LogError(ex.Message);

                throw;
            }


        }


        public List<KeyValuePair<string, string[]>> Distractors(string context)
        {
            try
            {

                scope.Set("context", context);
            scope.Set("s2vConst", s2vConst);

            scope.Exec(@"
distractors = sense2vec_get_words(context,s2vConst)
s2vlens=len(distractors)
");
            var distractors = scope.Get<PyList>("distractors");
                _logger.LogError($"{distractors}");
                var len = scope.Get<int>("s2vlens");

            List<KeyValuePair<string, string[]>> ret = new List<KeyValuePair<string, string[]>>();
            for (int i = 0; i < len; i++)
            {
                Console.WriteLine($"{distractors[i][0].ToString()}");
                Console.WriteLine($"{distractors[i][1].As<string[]>()}");

                ret.Add(new KeyValuePair<string, string[]>(distractors[i][0].ToString(), distractors[i][1].As<string[]>()));
            }
            return ret;
        }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex);
                        _logger.LogError(ex.StackTrace);
                        _logger.LogError(ex.Message);

                        throw;
                    }
}
        public List<string> Sent_Tokenizer(string context)
        {

            try
            { 

            scope.Set("passage356", context);
            scope.Exec(@"
sentences = split_passage_to_sentences(passage356)
print(sentences)

");
            var sentences = scope.Get<string[]>("sentences");


                return sentences.ToList() ;
}
                    catch (Exception ex)
                    {
    _logger.LogError(ex);
    _logger.LogError(ex.StackTrace);
    _logger.LogError(ex.Message);

    throw;
}
        }
        public ResultModel Genarate(contextModel contextModel)
        {
            using (Py.GIL())
            {
                IList<string> answers = new List<string>();
        

                answers = AnswerExtractionModel(contextModel.context);

                List<string> IntrogativeWords = new List<string>();
                List<string> NoIntrogativeWords = new List<string>();
                IList<string> questions=new List<string>();

                IList<string> questions2 = new List<string>();

                foreach (var answer in answers)
                {
                    if (contextModel.WH != false)
                    {
                    IntrogativeWords.Add(IntrogativeWordModel(contextModel.context, answer));
                    }
                        NoIntrogativeWords.Add("");



                }
                if (contextModel.WH != false)
                {
                    questions = QuestionGenrationModel(contextModel.context, answers, IntrogativeWords);
                    }

                List< KeyValuePair<string, string[]> > distactors=new List<KeyValuePair<string, string[]>>();
                if (contextModel.T_F != false|| contextModel.MCQ != false||contextModel.COMPLETE != false)
                {
                     distactors = Distractors(contextModel.context);
                }
                if (contextModel.T_F != false || contextModel.WH != false)
                {

                questions2 = QuestionGenrationModel(contextModel.context, answers, NoIntrogativeWords);
                }
                Dictionary<string, string> WH = new Dictionary<string, string>();
                Dictionary<string, string> T_F = new Dictionary<string, string>();
                Dictionary<string, string> MCQ = new Dictionary<string, string>();
                Dictionary<string, string> COMPLETE = new Dictionary<string, string>();
                if (contextModel.T_F != false )
                {

                    var boolqg = BoolQGModel(contextModel.context);
                    for (int i = 0; i < boolqg.Count; i++)
                    {
                        T_F.Add("Yes or No:\n" + boolqg[i], "yes");
                    }
                }

                var contextSentnces = Sent_Tokenizer(contextModel.context);
                for (int i = 0; i < answers.Count; i++)
                {
                    if (questions.Count>0)
                    {
                        if (questions[i].ToLower().Contains("what") || questions[i].ToLower().Contains("who") || questions[i].ToLower().Contains("where") || questions[i].ToLower().Contains("how") || questions[i].ToLower().Contains("when") || questions[i].ToLower().Contains("why") || questions[i].ToLower().Contains("which") || questions[i].ToLower().Contains("whom") || questions[i].ToLower().Contains("whose"))
                        {
                            if (!WH.ContainsKey("Elaborate:\n" + questions[i]))
                            {
                                WH.Add("Elaborate:\n" + questions[i], answers[i]);

                            }
                        }
                        else
                                if (!T_F.ContainsKey("True or False:\n" + questions[i]) && contextModel.T_F != false)
                        {

                            T_F.Add("True or False:\n" + questions[i].Replace("?", "."), "true");
                        }

                    }
                    if (questions2.Count > 0)
                    {
                       
                            if (questions2[i].ToLower().Contains("what") || questions2[i].ToLower().Contains("who") || questions2[i].ToLower().Contains("where") || questions2[i].ToLower().Contains("how") || questions2[i].ToLower().Contains("when") || questions2[i].ToLower().Contains("why") || questions2[i].ToLower().Contains("which") || questions2[i].ToLower().Contains("whom") || questions2[i].ToLower().Contains("whose"))
                        {
                            if (!WH.ContainsKey("Elaborate:\n" + questions2[i]))
                            {

                                if (contextModel.WH != false)
                                {

                                    WH.Add("Elaborate:\n" + questions2[i], answers[i]);
                                }
                            }
                            }
                            else
                                if (!T_F.ContainsKey("True or False:\n" + questions2[i]) && contextModel.T_F != false)
                            {

                                T_F.Add("True or False:\n" + questions2[i].Replace("?", "."), "true");
                            }


                        


                    }
                }
                for (int i = 0; i < distactors.Count; i++)
                {



                    for (int j = 0; j < contextSentnces.Count; j++)
                    {
            

                        if (contextSentnces[j].Contains(distactors[i].Key))
                        {
                            var choices = $"1) {distactors[i].Key}";
                            if (distactors[i].Value.Length<1)
                            {
                                if (!COMPLETE.ContainsKey("Complete:\n" + contextSentnces[j].Replace(distactors[i].Key, "__________"))&&contextModel.COMPLETE != false)
                                {

                                    COMPLETE.Add("Complete:\n" + contextSentnces[j].Replace(distactors[i].Key, "__________"), distactors[i].Key);
                                }
                                continue;
                            }
                            var k = 2;
                            if (!T_F.ContainsKey("True or False:\n" + contextSentnces[j]) && contextModel.T_F != false)
                            {

                                T_F.Add("True or False:\n" + contextSentnces[j], $"True");
                            }
                            foreach (var dis in distactors[i].Value)
                            {
                                if (!T_F.ContainsKey("True or False:\n" + (contextSentnces[j].Replace(distactors[i].Key, dis))) && contextModel.T_F != false)
                                {

                                    T_F.Add("True or False:\n" + contextSentnces[j].Replace(distactors[i].Key, dis), $"false corection:- {dis} => {distactors[i].Key}");
                                }
                                    if (k <= 5&& contextModel.MCQ != false)
                                    {

                                        choices += $"\n{k}) {dis}";
                                        k++;
                                    }
                            }
                            if (!MCQ.ContainsKey("MCQ:\n" + contextSentnces[j].Replace(distactors[i].Key, "__________") + $"\n{choices}")&& contextModel.MCQ != false)
                            {

                                MCQ.Add("MCQ:\n" + contextSentnces[j].Replace(distactors[i].Key, "__________") + $"\n{choices}", distactors[i].Key);
                            }
                            if (!COMPLETE.ContainsKey("Complete:\n" + contextSentnces[j].Replace(distactors[i].Key, "__________")) && contextModel.COMPLETE != false)
                            {

                                COMPLETE.Add("Complete:\n" + contextSentnces[j].Replace(distactors[i].Key, "__________"), distactors[i].Key);
                            }
                        }
                    }

                }
      

                var sad =WH.Concat(MCQ).Concat(COMPLETE).Concat(T_F).ToList();
                var sad2 = sad;
                var result = new ResultModel();
                result.QuestionsAnswers = sad2;

                return result;
            }
        }
    }
}
