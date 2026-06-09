import { Pipe, PipeTransform } from '@angular/core';
import { normalizeDisplayName } from '../utils/display-name.util';

@Pipe({
  name: 'displayName',
  standalone: true,
})
export class DisplayNamePipe implements PipeTransform {
  transform(value?: string | null): string {
    if (!value) {
      return '';
    }

    return normalizeDisplayName(value) || value.trim();
  }
}
