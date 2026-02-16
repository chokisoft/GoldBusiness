import { Pipe, PipeTransform } from '@angular/core';
import { TranslationService } from '../services/translation.service';

@Pipe({
  name: 'translate',
  pure: false // Permite que se actualice cuando cambia el idioma
})
export class TranslatePipe implements PipeTransform {
  constructor(private translationService: TranslationService) { }

  transform(key: string, ...params: any[]): string {
    return this.translationService.translate(key, params);
  }
}
