import { Component, HostListener, inject, OnInit, ViewChild } from '@angular/core';
import {TabsModule} from 'ngx-bootstrap/tabs';
import { FormsModule, NgForm } from '@angular/forms';
import { Member } from '../../_models/member';
import { AccountService } from '../../_services/account.service';
import { MembersService } from '../../_services/members.service';
import { ToastrService } from 'ngx-toastr';
<<<<<<< HEAD
=======
import { PhotoEditorComponent } from "../photo-editor/photo-editor.component";
>>>>>>> Parcial04


@Component({
  selector: 'app-member-edit',
  standalone: true,
<<<<<<< HEAD
  imports: [TabsModule, FormsModule],
=======
  imports: [TabsModule, FormsModule, PhotoEditorComponent],
>>>>>>> Parcial04
  templateUrl: './member-edit.component.html',
  styleUrl: './member-edit.component.css'
})

export class MemberEditComponent implements OnInit {
    @ViewChild("editForm") editForm?: NgForm;
    @HostListener("window:beforeunload", ["event"]) notify($event: any) {
        if (this.editForm?.dirty) {
          $event.returnValue = true;
        }
      }
    member?: Member;
<<<<<<< HEAD
    private accountService = inject(AccountService);
=======
    public accountService = inject(AccountService);
>>>>>>> Parcial04
    private membersService = inject(MembersService);
    private toastr = inject(ToastrService);
    ngOnInit(): void {
        this.loadMember();
    }

    loadMember() {
        const user = this.accountService.currentUser();
        if (!user) return;
        this.membersService.getMember(user.username).subscribe({
            next: member => this.member = member
        })
    }

    updateMember() {
        this.membersService.updateMember(this.editForm?.value).subscribe({
          next: _ => {
            this.toastr.success("Profile updated!");
            this.editForm?.reset(this.member); 
          }
        })
    }
<<<<<<< HEAD
=======

    onMemberChange(event: Member){
      this.member = event;
    }
>>>>>>> Parcial04
}