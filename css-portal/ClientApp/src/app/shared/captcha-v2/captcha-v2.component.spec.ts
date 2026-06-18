import { waitForAsync, ComponentFixture, TestBed } from "@angular/core/testing";
import { HttpClientTestingModule } from "@angular/common/http/testing";
import { FormsModule } from "@angular/forms";
import { CaptchaV2Component } from "./captcha-v2.component";
import { ConfigDataService } from "@services/config-data.service";

describe("CaptchaV2Component", () => {
  let component: CaptchaV2Component;
  let fixture: ComponentFixture<CaptchaV2Component>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [CaptchaV2Component],
      providers: [ConfigDataService],
      imports: [HttpClientTestingModule, FormsModule],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CaptchaV2Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });
});
