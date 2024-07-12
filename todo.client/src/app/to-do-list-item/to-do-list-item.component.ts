import { Component, Input, Output, EventEmitter } from '@angular/core';
import { ToDo } from '../models/Todo'

@Component({
  selector: 'app-to-do-list-item',
  templateUrl: './to-do-list-item.component.html',
  styleUrl: './to-do-list-item.component.css'
})
export class ToDoListItemComponent {
  @Input() todo!: ToDo;
  @Output() delete = new EventEmitter<string>();

  onDelete(): void {
    this.delete.emit(this.todo.id);
  }
}
