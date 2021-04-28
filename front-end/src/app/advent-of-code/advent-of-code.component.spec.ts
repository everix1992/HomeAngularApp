import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdventOfCodeComponent } from './advent-of-code.component';

describe('AdventOfCodeComponent', () => {
  let component: AdventOfCodeComponent;
  let fixture: ComponentFixture<AdventOfCodeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdventOfCodeComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdventOfCodeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
