import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { LogInComponent } from './logIn/logIn.component';
import {FormsModule} from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AppRoutes } from './routes';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HomeComponent } from './home/home.component';
import { MediaComponent } from './Media/Media.component';
import { NewsComponent } from './News/News.component';
import { RentComponent } from './rent/rent.component';
import { EditUserComponent } from './editUser/editUser.component';
import { MatDialogModule } from '@angular/material/dialog';


@NgModule({
  declarations: [							
    AppComponent,
      NavComponent,
      LogInComponent,
      HomeComponent,
      MediaComponent,
      NewsComponent,
      RentComponent,
      EditUserComponent
   ],
  imports: [
    RouterModule.forRoot(AppRoutes),
    BrowserModule,
    FormsModule,
    HttpClientModule,
    BsDropdownModule.forRoot(),
    BrowserAnimationsModule,
    MatDialogModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
