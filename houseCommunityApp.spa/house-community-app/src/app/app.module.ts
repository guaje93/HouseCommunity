import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { LogInComponent } from './logIn/logIn.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
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
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { ForgetPasswordComponent } from './forgetPassword/forgetPassword.component';
import { ResetPasswordComponent } from './resetPassword/resetPassword.component';
import { ChangePasswordComponent } from './changePassword/changePassword.component';
import { ProgressComponent } from './progress/progress.component';
import { DragAndDropDirective } from './directives/dragAndDrop.directive';
import { DatePipe } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { ImagePreviewComponent } from './ImagePreview/ImagePreview.component';
import { MatSortModule } from '@angular/material/sort';
import { MediaHistoryComponent } from './mediaHistory/MediaHistory.component';


@NgModule({
  declarations: [				
    AppComponent,
    NavComponent,
    LogInComponent,
    HomeComponent,
    MediaComponent,
    NewsComponent,
    RentComponent,
    EditUserComponent,
    ForgetPasswordComponent,
    ResetPasswordComponent,
    ChangePasswordComponent,
    ProgressComponent,
    DragAndDropDirective,
      ImagePreviewComponent,
      MediaHistoryComponent,
      MediaHistoryComponent,
      MediaHistoryComponent
   ],
  imports: [
    RouterModule.forRoot(AppRoutes),
    BrowserModule,
    FormsModule,
    HttpClientModule,
    BsDropdownModule.forRoot(),
    BrowserAnimationsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatTableModule,
    MatSortModule,
    MatSelectModule,
    MatButtonModule,
    ReactiveFormsModule,

  ],
  providers: [DatePipe],
  bootstrap: [AppComponent]
})
export class AppModule { }
