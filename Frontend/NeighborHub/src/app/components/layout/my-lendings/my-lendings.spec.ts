import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MyLendings } from './my-lendings';

describe('MyLendings', () => {
  let component: MyLendings;
  let fixture: ComponentFixture<MyLendings>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MyLendings]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MyLendings);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
