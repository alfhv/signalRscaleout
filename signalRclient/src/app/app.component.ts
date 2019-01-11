import { Component, OnInit } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr'; 

import { Message } from 'primeng/api';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'signalRclient';
  private _hubConnection: HubConnection;
  msgs: Message[] = [];
  backendServer: string = '';
  connectionStatus: string = '';
 
  ngOnInit(): void {
    /*
    //this._hubConnection = new HubConnectionBuilder().withUrl('http://localhost:5000/notify').build();
    this._hubConnection = new HubConnectionBuilder().withUrl('http://localhost:5000/SimpleHub').build();
    this._hubConnection
    .start()
    .then(() => console.log('Connection started!'))
    .catch(err => console.log('Error while establishing connection :('));

  this._hubConnection.on('BroadcastMessage', (type: string, payload: string) => {
    this.msgs.push({ severity: type, summary: payload });
  });*/
  }

  connectSignalR() {
    var backendHub = this.backendServer + '/SimpleHub';
    this._hubConnection = new HubConnectionBuilder().withUrl(backendHub).build();
    this._hubConnection
    .start()
    .then(() => { 
      console.log('Connection started!');
      this.connectionStatus = 'Connection started on hub: ' + backendHub;

      this._hubConnection.send('echo', this.connectionStatus);

    })
    .catch(err => { 
      console.log('Error while establishing connection :(');
      this.connectionStatus = 'Error while establishing connection on hub: ' + backendHub;
    });

  this._hubConnection.on('BroadcastMessage', (type: string, payload: string) => {
    this.msgs.push({ severity: type, summary: payload });
  });

  this._hubConnection.on('echo', (payload: string) => {
    this.msgs.push({ severity: 'success', summary: payload });
  });

  }
}
