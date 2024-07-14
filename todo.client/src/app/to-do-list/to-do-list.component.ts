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
  todosChecked: ToDo[] = [];

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
      this.todos = data.filter(options => !options.isChecked);
      this.todosChecked = data.filter(options => options.isChecked)

    })
  }

  deleteToDoItem(id: string): void {
    this.service.deleteToDo(id).subscribe(() => {
      this.todos = this.todos.filter(todo => todo.id !== id);

      const checkedIndex = this.todosChecked.findIndex(todo => todo.id === id);
      if (checkedIndex !== -1) {
        this.todosChecked.splice(checkedIndex, 1);
      }
    });
  }


  toggleChecked(todo: ToDo): void {
    this.service.toggleItem(todo).subscribe(() => {
      this.loadToDos();
    })
  }
}
