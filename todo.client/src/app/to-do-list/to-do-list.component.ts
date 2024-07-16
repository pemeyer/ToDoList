import { Component, OnInit } from '@angular/core';
import { ToDoService } from '../services/to-do.service';
import { MatSnackBar } from '@angular/material/snack-bar';

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
  errorMessage: string | null = null;

  constructor(private service: ToDoService, private snackBar: MatSnackBar) {

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

    this.service.addToDos(newToDo).subscribe({
      next: (todo: ToDo) => {
        this.todos.push(todo);
        this.newToDoTitle = '';
      },
      error: (error) => {
        this.showError(error.message);
      }
    });
  }


  loadToDos(): void {
    this.service.getToDos().subscribe({
      next: (data: ToDo[]) => {
        this.todos = data.filter(options => !options.isChecked);
        this.todosChecked = data.filter(options => options.isChecked);
      },
      error: (error) => {
        this.showError(error.message);
      }
    });
  }

  deleteToDoItem(id: string): void {
    this.service.deleteToDo(id).subscribe({
      next: () => {
        this.todos = this.todos.filter(todo => todo.id !== id);

        const checkedIndex = this.todosChecked.findIndex(todo => todo.id === id);
        if (checkedIndex !== -1) {
          this.todosChecked.splice(checkedIndex, 1);
        }
      },
      error: (error) => {
        this.showError(error.message);
      }
    });
  }



  toggleChecked(todo: ToDo): void {
    this.service.toggleItem(todo).subscribe({
      next: () => {
        this.loadToDos();
      },
      error: (error) => {
        this.showError(error.message);
      }
    });
  }

  showError(message: string): void {
    this.snackBar.open(message, 'Close', {
      duration: 5000,
      horizontalPosition: 'right',
      verticalPosition: 'top',
    });
  }
}
