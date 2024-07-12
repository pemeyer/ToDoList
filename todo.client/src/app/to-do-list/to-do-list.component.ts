import { Component, OnInit } from '@angular/core';
import { ToDoListItemComponent } from '../to-do-list-item/to-do-list-item.component'
import { ToDoService } from '../services/to-do.service';
import { ToDo } from '../models/Todo';

@Component({
  selector: 'app-to-do-list',
  templateUrl: './to-do-list.component.html',
  styleUrl: './to-do-list.component.css'
})
export class ToDoListComponent {
  todos: ToDo[] = [];
  newToDoTitle: string = '';

  constructor(private service :ToDoService) {

  }

  ngOnInit(): void {
    this.loadToDos();
  }

  addToDoItem(): void {

    if (!this.newToDoTitle.trim()) {
      return;
    }

    const newToDo: ToDo = {
      id: '',
      toDo: this.newToDoTitle,
      isChecked: false,
    };


    this.service.addToDos(newToDo).subscribe((todo: ToDo) => {
      console.log(todo);
      this.todos.push(todo);
      this.newToDoTitle = '';
    });
  }

  loadToDos(): void {
    this.service.getToDos().subscribe((data: ToDo[]) => {
      console.log(data);
      this.todos = data;
    })
  }

  deleteToDoItem(id: string): void {
    this.service.deleteToDo(id).subscribe(() => {
      this.todos = this.todos.filter(todo => todo.id !== id);
    })
  }
}
