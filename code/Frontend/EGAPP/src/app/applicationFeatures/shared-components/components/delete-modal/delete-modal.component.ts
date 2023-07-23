import { Component, OnInit, OnChanges, SimpleChanges, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'delete-modal',
  templateUrl: './delete-modal.component.html',
  styleUrls: ['./delete-modal.component.sass']
})
export class DeleteModalComponent implements OnInit {

  constructor() { }

  // ngOnChanges(simpleChanges: SimpleChanges) {
   
  // }

  ngOnInit(): void {
  }

  deleteItem() {
    this.delete.emit("");
  }
 
  @Input() deleteMessage: string;
  @Output() delete: EventEmitter<any> = new EventEmitter<any>();
}
