import { Component, input, OnInit, ViewEncapsulation } from '@angular/core';
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
export class MembersCardComponent implements OnInit{
  ngOnInit(): void {
    console.log("member: " + this.member().userName);
  }
  member = input.required<Member>();
}