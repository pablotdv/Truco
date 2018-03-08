import { Component, OnInit } from '@angular/core';
import { Pais } from '../pais';
import { PAISES } from '../mock-paises';
import { PaisService } from '../pais.service';

@Component({
  selector: 'app-paises',
  templateUrl: './paises.component.html',
  styleUrls: ['./paises.component.css']
})
export class PaisesComponent implements OnInit {

  selectedPais: Pais;

  paises: Pais[];

  constructor(private paisService: PaisService) { }

  ngOnInit() {
    this.getPaises();
  }

  onSelect(pais: Pais): void {
    this.selectedPais = pais;
  }

  getPaises(): void {
    this.paisService.getPaises()
      .subscribe(paises => this.paises = paises);
  }

}
