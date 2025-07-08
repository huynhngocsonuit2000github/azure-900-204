import { Component } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { TodoComponent } from './todo/todo.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [HttpClientModule, TodoComponent], // ✅ Add HttpClientModule here
  templateUrl: './app.component.html',
})
export class AppComponent { }
