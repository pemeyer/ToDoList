import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ToDo } from '../models/Todo';

@Injectable({
  providedIn: 'root'
})

export class ToDoService {

  private apiUrl = 'https://localhost:7247/ToDo';

  constructor(private http: HttpClient) { }


  addToDos(todo: ToDo): Observable<ToDo> {
    return this.http.post<ToDo>(this.apiUrl + '/CreateItem', todo);
  }

  getToDos(): Observable<ToDo[]> {
    return this.http.get<ToDo[]>(this.apiUrl).pipe(
      catchError(this.handleError)
    );
  }

  deleteToDo(id:string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`).pipe(
      catchError(this.handleError)
    );
  }

  toggleItem(todo: ToDo): Observable<any> {
    const url = `${this.apiUrl}/ToggleItem/${todo.id}`;
    const isChecked = !todo.isChecked;
    return this.http.patch(url, { isChecked }).pipe(
      catchError(this.handleError)
    );
  }

  private handleError(error: HttpErrorResponse) {
    let errorMessage = 'An unknown error occurred!';

    if (error.error instanceof ErrorEvent) {
      errorMessage = `Client-side error: ${error.error.message}`;
    } else {
      switch (error.status) {
        case 400:
          errorMessage = 'Bad Request: The server could not understand the request due to invalid syntax.';
          break;
        case 404:
          errorMessage = 'Not Found: The server could not find the requested resource.';
          break;
        default:
          errorMessage = `Server-side error: ${error.status}\nMessage: ${error.message}`;
          break;
      }
    }

    return throwError(() => new Error(errorMessage));
  }
} 
