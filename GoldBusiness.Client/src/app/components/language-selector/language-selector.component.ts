import { Component, OnInit, OnDestroy, ElementRef, Renderer2, ViewChild } from '@angular/core';
import { LanguageService, Language } from '../../services/language.service';
import { TranslationService } from '../../services/translation.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-language-selector',
  templateUrl: './language-selector.component.html',
  styleUrls: ['./language-selector.component.css']
})
export class LanguageSelectorComponent implements OnInit, OnDestroy {
  availableLanguages: Language[] = [];
  selectedLanguageCode: string = 'es';
  label: string = '';

  // Compact mode (icon-only) on small screens
  compact: boolean = false;
  popoverOpen: boolean = false;

  // Inline style object for popover positioning
  popoverStyle: { [key: string]: string } = {};

  private languageSubscription?: Subscription;
  private docClickUnlisten?: () => void;

  @ViewChild('compactBtn', { read: ElementRef }) compactBtn?: ElementRef;

  constructor(
    private languageService: LanguageService,
    private translationService: TranslationService,
    private host: ElementRef,
    private renderer: Renderer2
  ) {
    this.availableLanguages = this.languageService.availableLanguages;
    this.selectedLanguageCode = this.languageService.getCurrentLanguage();
    console.log('🎨 LanguageSelector inicializado con idioma:', this.selectedLanguageCode);
  }

  ngOnInit(): void {
    this.loadLabel();

    // Suscribirse a cambios de idioma
    this.languageSubscription = this.languageService.currentLanguage$.subscribe(langCode => {
      console.log('🔄 LanguageSelector: Idioma cambiado a', langCode);
      this.selectedLanguageCode = langCode;
      this.loadLabel();
    });

    // Init compact mode según ancho actual
    this.updateCompactMode();

    // Escucha clics en documento para cerrar popover si se clica fuera
    this.docClickUnlisten = this.renderer.listen('document', 'click', (event: MouseEvent) => {
      if (!this.popoverOpen) return;
      const clickedInside = this.host.nativeElement.contains(event.target);
      if (!clickedInside) {
        this.closePopover();
      }
    });

    // Escucha redimensiones de ventana
    window.addEventListener('resize', this.onResize);
  }

  ngOnDestroy(): void {
    this.languageSubscription?.unsubscribe();
    console.log('🧹 LanguageSelector destruido');
    if (this.docClickUnlisten) { this.docClickUnlisten(); }
    window.removeEventListener('resize', this.onResize);
  }

  private onResize = () => {
    const prevCompact = this.compact;
    this.updateCompactMode();
    // if popover open and compact, recompute position
    if (this.popoverOpen && this.compact && this.compactBtn) {
      this.computePopoverStyle();
    }
    // if compact state changed close popover to avoid stale position
    if (prevCompact !== this.compact) {
      this.closePopover();
    }
  }

  private updateCompactMode(): void {
    this.compact = window.innerWidth <= 768;
    if (!this.compact) {
      this.popoverOpen = false;
      this.popoverStyle = {};
    }
  }

  private loadLabel(): void {
    this.label = this.translationService.translate('language.label');
    console.log('📝 Label actualizado:', this.label);
  }

  changeLanguage(languageCode: string): void {
    console.log('🌍 Usuario cambió idioma a:', languageCode);
    this.languageService.setLanguage(languageCode);
    this.closePopover();
  }

  togglePopover(): void {
    this.popoverOpen = !this.popoverOpen;
    if (this.popoverOpen) {
      // slight delay to allow view to render the popover element if needed
      setTimeout(() => this.computePopoverStyle(), 0);
    } else {
      this.popoverStyle = {};
    }
  }

  closePopover(): void {
    this.popoverOpen = false;
    this.popoverStyle = {};
  }

  getCurrentLanguageInfo(): Language {
    return this.languageService.getCurrentLanguageInfo();
  }

  private computePopoverStyle(): void {
    try {
      const btnEl = this.compactBtn?.nativeElement as HTMLElement | undefined;
      if (!btnEl) return;
      const rect = btnEl.getBoundingClientRect();

      const desiredMinWidth = 140; // match desktop select min width
      let width = Math.max(desiredMinWidth, rect.width);
      const spacing = 8; // margin from screen edges

      // if would overflow right edge, shift left
      let left = rect.left;
      if (left + width + spacing > window.innerWidth) {
        left = Math.max(spacing, window.innerWidth - width - spacing);
      }

      // if left too small, clamp
      left = Math.max(spacing, left);

      // compute top just under navbar / button
      const top = rect.bottom + 6; // small gap

      this.popoverStyle = {
        position: 'fixed',
        top: `${Math.ceil(top)}px`,
        left: `${Math.ceil(left)}px`,
        minWidth: `${Math.ceil(width)}px`,
        zIndex: '20000'
      };
    } catch (err) {
      console.warn('Could not compute popover position', err);
      this.popoverStyle = {};
    }
  }
}
