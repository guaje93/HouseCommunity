import { HttpClient } from '@angular/common/http';
import { EventEmitter, Injectable } from '@angular/core';  
import * as signalR from "@aspnet/signalr";
import { Message } from '../Model/message';  

@Injectable({
  providedIn: 'root'
})

export class ChatService {  
  messageReceived = new EventEmitter<Message>();  
  connectionEstablished = new EventEmitter<Boolean>();  
  
  baseUrl = 'http://localhost:5000/api/chat/';
  private connectionIsEstablished = false;  
  private _hubConnection: signalR.HubConnection;  
  
  constructor(private http: HttpClient) {  
    this.createConnection();  
    this.registerOnServerEvents();  
    this.startConnection();  
  }  
  
  sendMessage(message: Message) {  
    this._hubConnection.invoke('NewMessage', message);  
  }  
  
  private createConnection() {  
    this._hubConnection = new signalR.HubConnectionBuilder()  
      .withUrl("http://localhost:5000/MessageHub")  
      .build();  
  }  
  
  private startConnection(): void {  
    this._hubConnection  
      .start()  
      .then(() => {  
        this.connectionIsEstablished = true;  
        console.log('Hub connection started');  
        this.connectionEstablished.emit(true);  
      })  
      .catch(err => {  
        console.log('Error while establishing connection, retrying...');  
        setTimeout(function () { this.startConnection(); }, 5000);  
      });  
  }  
  
  private registerOnServerEvents(): void {  
    this._hubConnection.on('MessageReceived', (data: any) => {  
      this.messageReceived.emit(data);  
    });  
  }
  
  public getConversations(id: number){
return this.http.get(this.baseUrl + id);
  }

    
  public loadMessages(id: number, userId: number){
    return this.http.get(this.baseUrl + "get-messages/" + id + '/' + userId);
      }

      public readMessage(model: any){
        return this.http.post(this.baseUrl + "read-message/", model);
          }

      public saveMessage(conversationId: number, userId: number, receiverId: number, message: Message){
        let model: any={};
        model.conversationId = conversationId;
        model.userId = userId;
        model.receiverId = receiverId;
        model.message = message.message;
        return this.http.post(this.baseUrl + "save-message/", model);
          }
} 