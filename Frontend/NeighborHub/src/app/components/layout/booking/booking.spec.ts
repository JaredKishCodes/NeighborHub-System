import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BookingComponent } from './booking';

describe('BookingComponent', () => {
  let component: BookingComponent;
  let fixture: ComponentFixture<BookingComponent>;
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [BookingComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BookingComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
