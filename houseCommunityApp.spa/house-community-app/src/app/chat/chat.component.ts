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
        isBuildingSame: c.isBuildingSame,
        avatarUrl: c.avatarUrl,
        notReadMessages: c.notReadMessages,
        modificationDate: c.modificationDate
      };
      return conv;
    }).sort((a, b) => {
      if (a.modificationDate === b.modificationDate) { return 0; }
      else if (a.modificationDate === null) { return 1; }
      else if (b.modificationDate === null) { return -1; }
      else { return a.modificationDate > b.modificationDate ? -1 : 1; }
    });

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

      this.chatService.saveMessage(this.currentChatId, this.authService.decodedToken.nameid, this.senderId, this.message).subscribe(data => {

      });
    }
  }
  public trackItem(index: number, item: any) {
    return item.id;
  }

  loadPrivateChat(element: any) {

    this.chatService.loadMessages(element.id, this.authService.decodedToken.nameid).subscribe(data => {
      console.log(data);
      this.sender = element.firstName + " " + element.lastName;
      this.senderId = element.id;
      this.messages = [];
      this.currentChatId = (data as any)?.id;
      console.log(this.currentChatId);
      if(this.currentChatId){
        let model: any = {};
        model.userId = this.authService.decodedToken.nameid;
        model.conversationId = this.currentChatId;
        this.chatService.readMessage(model).subscribe(data => 
          console.log(data))
      }
      if (data as any[]) {

        (data as any).messages.forEach(element => {
          let message = new Message();
          message.date = element.date;
          message.message = element.content;
          message.type = element.type;
          this.messages.push(message);
        });
        element.notReadMessages = 0;
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
