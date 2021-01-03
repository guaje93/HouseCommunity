import { HttpClient } from '@angular/common/http';
import { EventEmitter, Injectable } from '@angular/core';
import * as signalR from "@aspnet/signalr";
import { Message } from '../Model/message';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class ChatService {
  messageReceived = new EventEmitter<Message>();
  connectionEstablished = new EventEmitter<Boolean>();

  baseUrl = 'https://housecommunityapp.azurewebsites.net/api/chat/';
  private connectionIsEstablished = false;
  private _hubConnection: signalR.HubConnection;
  public subject = new BehaviorSubject(1);

  constructor(private http: HttpClient) {

  }

  sendMessage(message: Message) {
    this._hubConnection.invoke('NewMessage', message);
  }

  public createConnection() {
    this._hubConnection = new signalR.HubConnectionBuilder()
      .withUrl("https://housecommunityapp.azurewebsites.net/MessageHub")
      .build();
  }

  public startConnection(id: number): void {
    this._hubConnection
      .start()
      .then(() => {
        this.connectionIsEstablished = true;
        console.log('Hub connection started');
        this.connectionEstablished.emit(true);
      })
      .then(res => {
        this.joinGroups(id).subscribe(data => console.log(data));
      })
      .catch(err => {
        console.log('Error while establishing connection, retrying...');
        setTimeout(function () { this.startConnection(); }, 5000);
      });
  }

  public registerOnServerEvents(method: (...args: any[]) => void): void {
    this._hubConnection.on('MessageReceived',method);
  }

  public getConversations(id: number) {
    return this.http.get(this.baseUrl + id);
  }

  getNotReadMessages(id: number) {
    return this.http.get(this.baseUrl + 'not-read-messages/' + id);
  }

  public loadMessages(id: number, userId: number) {
    return this.http.get(this.baseUrl + "get-messages/" + id + '/' + userId);
  }

  public readMessage(model: any) {
    return this.http.post(this.baseUrl + "read-message/", model);
  }

  public joinGroups(id: number) {
    return this.http.get(this.baseUrl + "join-groups/" + id);
  }

  public leaveGroups(id: number) {
    return this.http.get(this.baseUrl + "leave-groups/" + id);
  }

  public saveMessage(conversationId: number, userId: number, receiverId: number, message: Message) {
    let model: any = {};
    model.conversationId = conversationId;
    model.userId = userId;
    model.receiverId = receiverId;
    model.message = message.message;
    return this.http.post(this.baseUrl + "save-message/", model);
  }
} 