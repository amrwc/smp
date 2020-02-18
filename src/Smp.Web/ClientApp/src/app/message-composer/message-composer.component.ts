import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-message-composer',
  templateUrl: './message-composer.component.html',
  styleUrls: ['./message-composer.component.scss']
})
export class MessageComposerComponent implements OnInit {

  public friends: Array<string> = [ "john", "jessie", "julie", "ryan", "tom" ];
  public friend = new FormControl('friend');

  constructor() { }

  ngOnInit(): void {
  }

}
