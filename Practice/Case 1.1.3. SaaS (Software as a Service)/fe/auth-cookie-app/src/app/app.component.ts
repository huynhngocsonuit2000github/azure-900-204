// src/app/app.component.ts
import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { PostListComponent } from './post-list/post-list.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, PostListComponent],
  template: `
    <div class="global-wrapper">
      <app-post-list></app-post-list> <!-- Always visible -->
      <router-outlet></router-outlet>
    </div>
  `
})
export class AppComponent { }
