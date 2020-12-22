import { MapType } from '@angular/compiler';
import { ChangeDetectorRef, Component, NgZone, OnInit } from '@angular/core';
import { Message } from '../Model/message';
import { AuthService } from '../_services/auth.service';
import { ChatService } from '../_services/chat.service';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss']
})
export class ChatComponent implements OnInit {

  title = 'ClientApp';
  txtMessage: string = '';
  uniqueID: string = new Date().getTime().toString();
  messages = new Array<Message>();
  message = new Message();
  conversationsList: any[];
  filteredConversations: any[];
  option: string = "0";
  sender: string;
  senderId: number;
  currentChatId: number;
  constructor(
    private chatService: ChatService,
    private _ngZone: NgZone,
    private authService: AuthService,
    private changeDetection: ChangeDetectorRef,
    private userService: UserService

  ) {
    this.subscribeToEvents();
  }
  ngOnInit(): void {

    this.chatService.getConversations(this.authService.decodedToken.nameid).subscribe(
      data => {
        console.log(data);
        this.conversationsList = data as any[];
        this.filteredConversations = this.FilterConversation(1).filter(p => p.isBuildingSame);
      })
  };

  private FilterConversation(type: number) {
    return this.conversationsList.filter(p => (p.userRole === type)).map(c => {
      let conv = {
        id: c.id,
        name: c.name,
        type: this.MapType(type),
        firstName: c.firstName,
        lastName: c.lastName,
        isBuildingSame: c.isBuildingSame
      };
      return conv;
    }
    );
  }
  MapType(type: number): string {
    switch (type) {
      case 1: { return "Mieszkaniec" }
      case 2: { return "Administracja" }
      case 3: { return "Zarządca" }
    }
  }

  sendMessage(): void {
    if (this.txtMessage) {
      this.message = new Message();
      this.message.clientuniqueid = this.uniqueID;
      this.message.type = "sent";
      this.message.message = this.txtMessage;
      this.message.date = new Date();
      this.messages.push(this.message);
      this.chatService.sendMessage(this.message);
      this.txtMessage = '';
      this.chatService.saveMessage(this.currentChatId, this.authService.decodedToken.nameid, this.senderId, this.message).subscribe(data => {

      });
    }
  }
  public trackItem (index: number, item: any) {
    return item.id;
  }

loadPrivateChat(id: number, firstName: string, lastName: string){

  this.chatService.loadMessages(id,this.authService.decodedToken.nameid).subscribe(data => {
  console.log(data);
  this.sender = firstName + " " + lastName;
  this.senderId = id;
  this.messages= [];
  this.currentChatId = (data as any)?.id;
  console.log(this.currentChatId);
  if(data as any[]){

    (data as any).messages.forEach(element => {
      let message= new Message();
      message.date = element.date;
      message.message = element.content;
      message.type = element.type;
      this.messages.push(message);
    });
    this.changeDetection.detectChanges();
  }

  console.log(this.messages)
})
}

  optionChange() {
    console.log(this.option);
    switch (this.option) {
      case "0":
        {
          this.filteredConversations = this.FilterConversation(1).filter(p => p.isBuildingSame);
          this.changeDetection.detectChanges();
          break;
        }
      case "1":
        {
          this.filteredConversations = this.FilterConversation(1);
          this.changeDetection.detectChanges();
          break;
        }
      case "2":
        {
          this.filteredConversations = this.FilterConversation(3);
          this.changeDetection.detectChanges();
          break;
        }
      case "3":
        {
          this.filteredConversations = this.FilterConversation(2);
          this.changeDetection.detectChanges();
          break;
        }
      }
  }

  private subscribeToEvents(): void {

    this.chatService.messageReceived.subscribe((message: Message) => {
      this._ngZone.run(() => {
        if (message.clientuniqueid !== this.uniqueID) {
          message.type = "received";
          this.messages.push(message);
        }
      });
    });
  }
}
