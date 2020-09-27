import { Routes } from "@angular/router";
import { ForgetPasswordComponent } from './forgetPassword/forgetPassword.component';
import { HomeComponent } from './home/home.component';
import { LogInComponent } from './logIn/logIn.component';
import { MediaComponent } from './Media/Media.component';
import { NewsComponent } from './News/News.component';
import { RentComponent } from './rent/rent.component';
import { ResetPasswordComponent } from './resetPassword/resetPassword.component';
import { AuthGuard } from './_guards/auth.guard';

export const AppRoutes: Routes = [


    { path: 'login', component: LogInComponent },
    { path: 'forgotPassword', component: ForgetPasswordComponent },
    { path: 'response-reset-password/:token', component: ResetPasswordComponent },
    { path: '', component: LogInComponent },
    {
        path: "",
        runGuardsAndResolvers: "always",
        canActivate: [AuthGuard],
        children: [
            { path: "home", component: HomeComponent },
            { path: "media", component: MediaComponent },
            { path: "news", component: NewsComponent },
            { path: "rent", component: RentComponent },
        ]
    },
    { path: '**', redirectTo: '/login', pathMatch: 'full' },
];