import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PaisDetailComponent } from './pais-detail.component';

describe('PaisDetailComponent', () => {
  let component: PaisDetailComponent;
  let fixture: ComponentFixture<PaisDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PaisDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PaisDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
