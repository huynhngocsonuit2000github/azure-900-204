import { Component, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TodoService } from './todo.service';
import { Todo } from './todo.model';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  todos = signal<Todo[]>([]);
  title = '';
  loading = false;
  constructor(private api: TodoService) { }
  ngOnInit() { this.refresh(); }
  refresh() {
    this.loading = true;
    this.api.list().subscribe({
      next: d => { this.todos.set(d); this.loading = false; },
      error: _ => this.loading = false
    });
  }
  add() {
    const t = this.title.trim(); if (!t) return;
    this.api.add({ title: t, isDone: false }).subscribe(_ => { this.title = ''; this.refresh(); });
  }
  toggle(todo: Todo) {
    const updated = { ...todo, isDone: !todo.isDone };
    this.api.update(todo.id!, updated).subscribe(_ => this.refresh());
  }
  onRemove(todo: Todo) {
    this.api.remove(todo.id!).subscribe(() => this.refresh());
  }
}
