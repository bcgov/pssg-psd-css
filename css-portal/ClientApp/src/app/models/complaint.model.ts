import { Complainant } from './complainant.model';
import { Property } from './property.model';

export class Complaint {
    property: Property;
    includeComplainant: boolean;
    complainant: Complainant;
}
