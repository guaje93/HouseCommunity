import { MapType } from '@angular/compiler';
import { ChangeDetectorRef, Component, NgZone, OnInit } from '@angular/core';
import { Message } from '../Model/message';
import { Role } from '../Model/Role';
import { AuthService } from '../_services/auth.service';
import { ChatService } from '../_services/chat.service';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss']
})
export class ChatComponent implements OnInit {

  txtMessage: string = '';
  uniqueID: string = new Date().getTime().toString();
  messages = new Array<Message>();
  message = new Message();
  conversationsList: any[];
  filteredConversations: any[];
  option: string;
  sender: string;
  senderId: number;
  currentChatId: number;
  role: Role;
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
    
    this.chatService.createConnection();
    this.chatService.startConnection(this.authService.decodedToken.nameid);

    this.chatService.registerOnServerEvents((data) => {
      if(this.currentChatId === (data as any).id)  {
        console.log(this.filteredConversations)
        let conv = this.filteredConversations.filter(p => p.conversationId === (data as any).id )[0];
        console.log(data);
        console.log(conv);
        this.loadPrivateChat(conv);
      }
      });
    this.role = this.authService.user.role;

    this.chatService.getConversations(this.authService.decodedToken.nameid).subscribe(
      data => {
        console.log(data);
        this.conversationsList = data as any[];
        if (this.role === 1 || this.role === 3) {
          this.option = "0";
          this.filteredConversations = this.FilterConversation(1).filter(p => p.isBuildingSame);
        }
        else {
          this.option = '1';
          this.filteredConversations = this.FilterConversation(1);
        }
      })
  };
  public readMessage(){
    if (this.currentChatId) {
      let model: any = {};
      model.userId = this.authService.decodedToken.nameid;
      model.conversationId = this.currentChatId;
      
      this.chatService.readMessage(model).subscribe(data =>
        {
          console.log(data)
          this.chatService.subject.next(456);
          
          
      })
      }
  }
  private FilterConversation(type: number) {
    return this.conversationsList.filter(p => (p.userRole === type)).map(c => {
      let conv = {
        id: c.id,
        conversationId: c.conversationId,
        name: c.name,
        type: type,
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
this.txtMessage = '';
      });
    }
  }
  public trackItem(index: number, item: any) {
    return item.id;
  }

  readPrivateChat(element: any){

    this.chatService.loadMessages(element.id, this.authService.decodedToken.nameid).subscribe(data => {
      console.log(data);
      this.sender = element.firstName + " " + element.lastName;
      this.senderId = element.id;
      this.messages = [];
      this.currentChatId = (data as any)?.id;
      console.log(this.currentChatId);
      
      if (data as any[]) {

        (data as any).messages.forEach(element => {
          let message = new Message();
          message.date = element.date;
          message.message = element.content;
          message.type = element.type;
          this.messages.push(message);
        });
        this.changeDetection.detectChanges();
        this.readMessage();
        this.filteredConversations.filter(p => p.conversationId === this.currentChatId)[0].notReadMessages = 0;
        this.conversationsList.filter(p => p.conversationId === this.currentChatId)[0].notReadMessages = 0;
      }
    }
    )}

  loadPrivateChat(element: any) {

    this.chatService.loadMessages(element.id, this.authService.decodedToken.nameid).subscribe(data => {
      console.log(data);
      this.sender = element.firstName + " " + element.lastName;
      this.senderId = element.id;
      this.messages = [];
      this.currentChatId = (data as any)?.id;
      console.log(this.currentChatId);
      
      if (data as any[]) {

        (data as any).messages.forEach(element => {
          let message = new Message();
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
