import { Component, OnInit } from '@angular/core';
import { Pais } from '../pais';
import { PaisService } from '../pais.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  paises: Pais[] = [];

  constructor(private paisService: PaisService) { }

  ngOnInit() {
    this.getPaises();
  }

  getPaises(): void {
    this.paisService.getPaises()
      .subscribe(paises => this.paises = paises.slice(1, 5));
  }
}
