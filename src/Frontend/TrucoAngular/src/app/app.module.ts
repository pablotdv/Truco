import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { PaisesComponent } from './paises/paises.component';
import { PaisDetailComponent } from './pais-detail/pais-detail.component';


@NgModule({
  declarations: [
    AppComponent,
    PaisesComponent,
    PaisDetailComponent
  ],
  imports: [
    BrowserModule,
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
