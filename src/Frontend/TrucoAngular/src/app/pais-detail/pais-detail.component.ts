import { Component, OnInit, Input } from '@angular/core';
import { Pais } from '../pais';

@Component({
  selector: 'app-pais-detail',
  templateUrl: './pais-detail.component.html',
  styleUrls: ['./pais-detail.component.css']
})
export class PaisDetailComponent implements OnInit {

  @Input() pais: Pais;

  constructor() { }

  ngOnInit() {
  }

}
