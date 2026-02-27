import { Component, Input, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { TranslationService } from '../../services/translation.service';

export type LoaderType = 'loading' | 'saving' | 'deleting' | 'processing';

@Component({
  selector: 'app-loader',
  templateUrl: './loader.component.html',
  styleUrls: ['./loader.component.css']
})
export class LoaderComponent implements OnInit, OnDestroy {
  @Input() message: string = '';
  @Input() translateKey?: string;
  @Input() size: number = 24;
  @Input() color: string = '#2563eb';
  @Input() center: boolean = false;
  @Input() inline: boolean = false;
  @Input() overlay: boolean = false;
  @Input() minimal: boolean = false;
  @Input() type: LoaderType = 'loading';
  @Input() showPulse: boolean = true;
  @Input() showProgress: boolean = false;

  currentMessage: string = '';
  private translationSubscription?: Subscription;

  constructor(private translationService: TranslationService) { }

  ngOnInit(): void {
    this.updateMessage();

    // Suscribirse a cambios de idioma
    this.translationSubscription = this.translationService.translations$
      .subscribe(() => {
        console.log('🔄 Loader: Idioma cambiado, actualizando mensaje...');
        this.updateMessage();
      });
  }

  ngOnDestroy(): void {
    this.translationSubscription?.unsubscribe();
  }

  private updateMessage(): void {
    if (this.message?.trim()) {
      // Si hay mensaje fijo, lo usamos directamente
      this.currentMessage = this.message;
    } else if (this.translateKey) {
      // Si hay translateKey, lo traducimos
      this.currentMessage = this.translationService.translate(this.translateKey);
    } else {
      // Si no hay nada, usamos la clave por defecto según el tipo
      const defaultKey = this.getDefaultTranslationKey();
      this.currentMessage = this.translationService.translate(defaultKey);
    }
  }

  get displayedMessage(): string {
    return this.currentMessage;
  }

  get spinnerStyle() {
    const spinnerSize = this.size * 1.5;
    return {
      width: `${spinnerSize}px`,
      height: `${spinnerSize}px`
    };
  }

  private getDefaultTranslationKey(): string {
    switch (this.type) {
      case 'saving': return 'common.saving';
      case 'deleting': return 'common.deleting';
      case 'processing': return 'common.processing';
      default: return 'common.loading';
    }
  }
}
