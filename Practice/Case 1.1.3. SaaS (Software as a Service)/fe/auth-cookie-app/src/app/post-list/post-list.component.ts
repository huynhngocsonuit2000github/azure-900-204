// src/app/components/post-list/post-list.component.ts
import { Component, OnInit } from '@angular/core';
import { Post } from '../../models/post.model';
import { AuthService } from '../auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-post-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './post-list.component.html'
})
export class PostListComponent implements OnInit {
  posts: Post[] = [];

  constructor(private authService: AuthService) { }

  ngOnInit() {
    this.authService.getPosts().subscribe({
      next: (data) => {
        console.log(data);

        this.posts = data
      },
      error: (err) => console.error('❌ Error loading posts', err)
    });
  }
}
