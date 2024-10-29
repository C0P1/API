import { Component, input } from '@angular/core';
import { Member } from '../../_models/member';

@Component({
  selector: 'app-members-card',
  standalone: true,
  imports: [],
  templateUrl: './member-card.component.html',
  styleUrl: './member-card.component.css'
})
export class MembersCardComponent {
  member = input.required<Member>();
}