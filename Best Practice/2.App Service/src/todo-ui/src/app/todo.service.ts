import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../environments/environment';
import { Observable } from 'rxjs';
import { Todo } from './todo.model';

@Injectable({ providedIn: 'root' })
export class TodoService {
    private base = environment.api;
    constructor(private http: HttpClient) { }
    list(): Observable<Todo[]> { return this.http.get<Todo[]>(`${this.base}`); }
    get(id: number): Observable<Todo> { return this.http.get<Todo>(`${this.base}/${id}`); }
    add(t: Partial<Todo>): Observable<Todo> { return this.http.post<Todo>(`${this.base}`, t); }
    update(id: number, t: Todo) { return this.http.put(`${this.base}/${id}`, t); }
    remove(id: number) { return this.http.delete(`${this.base}/${id}`); }
}
