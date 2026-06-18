import { Complainant } from './complainant.model';
import { ComplaintDetails } from './complaint-details.model';

export class Complaint {
    details: ComplaintDetails;
    complainant: Complainant;

    /**
     * The captcha token returned by a successful recaptcha call. The API performs additional verification of this value.
     *
     * @type {string}
     */
    captcha: string;
}
