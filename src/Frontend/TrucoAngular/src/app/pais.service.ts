import { Injectable } from '@angular/core';
import { MessageService } from './message.service';
import { Observable } from 'rxjs/Observable';
import { of } from 'rxjs/observable/of';
import { Pais } from './pais';
import { PAISES } from './mock-paises';

@Injectable()
export class PaisService {

  constructor(private messageService: MessageService) { }

  getPaises(): Observable<Pais[]> {
    this.messageService.add('PaisService: feteched paises');
    return of(PAISES);
  }

}
