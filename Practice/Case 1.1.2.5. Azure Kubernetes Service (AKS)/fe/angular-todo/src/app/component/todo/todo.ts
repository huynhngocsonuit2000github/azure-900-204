import { FormsModule } from '@angular/forms'; // Add this to imports
import { Todo } from '../../models/todo.model';
import { TodoService } from '../../service/todo';
import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
// ... other imports

@Component({
  selector: 'app-todo',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './todo.html',
  styleUrls: ['./todo.css']
})
export class TodoComponent implements OnInit {
  todos: Todo[] = [];

  newTodo: Partial<Todo> = { title: '', isCompleted: false };
  editingTodoId: number | null = null;

  constructor(private todoService: TodoService) { }

  ngOnInit(): void {
    this.loadTodos();
  }

  loadTodos() {
    this.todoService.getTodos().subscribe({
      next: (data) => this.todos = data,
      error: (err) => console.error('Error loading todos:', err)
    });
  }

  saveTodo() {
    if (this.editingTodoId) {
      // Update
      const updated: Todo = {
        id: this.editingTodoId,
        title: this.newTodo.title!,
        isCompleted: this.newTodo.isCompleted!
      };
      this.todoService.updateTodo(updated).subscribe(() => {
        this.resetForm();
        this.loadTodos();
      });
    } else {
      // Create
      this.todoService.createTodo(this.newTodo).subscribe(() => {
        this.resetForm();
        this.loadTodos();
      });
    }
  }

  deleteTodo(id: number): void {
    if (confirm('Are you sure you want to delete this todo?')) {
      this.todoService.deleteTodo(id).subscribe(() => {
        this.loadTodos();
        // Clear the form if the deleted item was being edited
        if (this.editingTodoId === id) {
          this.resetForm();
        }
      });
    }
  }

  toggleComplete(todo: Todo): void {
    const updated = { ...todo, isCompleted: !todo.isCompleted };
    this.todoService.updateTodo(updated).subscribe(() => {
      this.loadTodos();

      // If we're editing the same todo, update the form too
      if (this.editingTodoId === todo.id) {
        this.newTodo.isCompleted = updated.isCompleted;
      }
    });
  }


  editTodo(todo: Todo) {
    this.newTodo = { ...todo };
    this.editingTodoId = todo.id;
  }

  resetForm() {
    this.newTodo = { title: '', isCompleted: false };
    this.editingTodoId = null;
  }
}
