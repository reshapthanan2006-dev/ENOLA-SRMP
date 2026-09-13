import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

import {
  NavbarComponent
} from '../../shared/components/navbar/navbar.component';

@Component({
  selector: 'app-employer-layout',
  standalone: true,
  imports: [
    RouterOutlet,
    NavbarComponent
  ],
  templateUrl: './employer-layout.component.html',
  styleUrl: './employer-layout.component.css'
})
export class EmployerLayoutComponent {}