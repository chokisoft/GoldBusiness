import { Pipe, PipeTransform, ChangeDetectorRef, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { TranslationService } from '../services/translation.service';

@Pipe({
  name: 'translate',
  pure: false // Permite que se actualice cuando cambia el idioma
})
export class TranslatePipe implements PipeTransform, OnDestroy {
  private subscription: Subscription;

  constructor(private translationService: TranslationService, private cdr: ChangeDetectorRef) {
    // Suscribirse a cambios de idioma/traducciones para forzar actualización de la plantilla
    this.subscription = this.translationService.translations$.subscribe(() => {
      // Marca la vista para chequeo cuando cambie el idioma
      try { this.cdr.markForCheck(); } catch { /* ignore */ }
    });
  }

  transform(key: string, ...params: any[]): string {
    return this.translationService.translate(key, params);
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
