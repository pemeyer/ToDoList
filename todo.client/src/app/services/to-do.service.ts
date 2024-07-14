import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
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
    return this.http.get<ToDo[]>(this.apiUrl)
  }

  deleteToDo(id:string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  toggleItem(todo: ToDo): Observable<any> {
    const url = `${this.apiUrl}/ToggleItem/${todo.id}`;
    const isChecked = !todo.isChecked;
    return this.http.patch(url, { isChecked });
  }
} 
