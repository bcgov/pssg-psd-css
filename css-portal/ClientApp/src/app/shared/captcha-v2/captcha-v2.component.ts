import { Component, EventEmitter, OnInit, Output } from "@angular/core";
import { ConfigDataService, ServerConfig } from "@services/config-data.service";

@Component({
  selector: "app-captcha-v2",
  templateUrl: "./captcha-v2.component.html",
  styleUrls: ["./captcha-v2.component.scss"],
})
export class CaptchaV2Component implements OnInit {
  @Output() captchaResponse = new EventEmitter<CaptchaResponse>();

  captchaKey: string | null = null;

  constructor(private configDataService: ConfigDataService) {
    this.configDataService.getServerConfig().subscribe({
      next: (config: ServerConfig | null) => {
        this.captchaKey = config?.captcha?.key ?? null;
      },
      error: (error: any) => {
        console.error(error);
      },
    });
  }

  ngOnInit(): void {}

  resolved($event: any) {
    this.captchaResponse.emit({
      type: CaptchaResponseType.success,
      resolved: $event,
    });
  }

  errored($event: any) {
    this.captchaResponse.emit({
      type: CaptchaResponseType.error,
      error: $event,
    });
  }
}

export interface CaptchaResponse {
  type: CaptchaResponseType;
  resolved?: string;
  error?: string;
}

export enum CaptchaResponseType {
  success = "SUCCESS",
  error = "ERROR",
}
