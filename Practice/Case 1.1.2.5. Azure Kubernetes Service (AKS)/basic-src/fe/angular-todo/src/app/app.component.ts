import { Component } from '@angular/core';
import { TodoComponent } from './todo/todo.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [TodoComponent], // ✅ Add HttpClientModule here
  templateUrl: './app.component.html',
})
export class AppComponent { }
