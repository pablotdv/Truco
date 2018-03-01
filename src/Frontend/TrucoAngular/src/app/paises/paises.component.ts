import { Component, OnInit } from '@angular/core';
import { Pais } from '../pais';
import { PAISES } from '../mock-paises';

@Component({
  selector: 'app-paises',
  templateUrl: './paises.component.html',
  styleUrls: ['./paises.component.css']
})
export class PaisesComponent implements OnInit {

  paises = PAISES;

  constructor() { }

  ngOnInit() {
  }

}
