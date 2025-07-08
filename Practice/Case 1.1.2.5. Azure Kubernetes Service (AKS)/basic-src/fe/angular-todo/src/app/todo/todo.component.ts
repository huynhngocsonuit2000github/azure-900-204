import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TodoService } from '../todo.service';
import { Todo } from '../todo.model';

@Component({
  selector: 'app-todo',
  standalone: true, // ✅ this makes it a standalone component
  imports: [CommonModule], // ✅ if using *ngFor, *ngIf
  templateUrl: './todo.component.html',
})
export class TodoComponent implements OnInit {
  todos: Todo[] = [];

  constructor(private todoService: TodoService) { }

  ngOnInit() {
    this.todoService.getTodos().subscribe((data: Todo[]) => {
      this.todos = data;
    });
  }
}
