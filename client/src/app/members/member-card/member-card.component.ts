import { Component, input, ViewEncapsulation } from '@angular/core';
import { Member } from '../../_models/member';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-members-card',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './member-card.component.html',
  styleUrl: './member-card.component.css',
  encapsulation: ViewEncapsulation.None
})
export class MembersCardComponent {
  member = input.required<Member>();
}