import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { MatSnackBarModule } from '@angular/material/snack-bar';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ToDoListItemComponent } from './to-do-list-item/to-do-list-item.component';
import { ToDoListComponent } from './to-do-list/to-do-list.component';
import { ToDoService }  from './services/to-do.service';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async'

@NgModule({
  declarations: [
    AppComponent,
    ToDoListItemComponent,
    ToDoListComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    AppRoutingModule,
    MatSnackBarModule
  ],
  providers: [ToDoService, provideAnimationsAsync()],
  bootstrap: [AppComponent]
})
export class AppModule { }
