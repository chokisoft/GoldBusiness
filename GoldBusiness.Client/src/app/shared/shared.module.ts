import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

// Pipes compartidos
import { TranslatePipe } from '../pipes/translate.pipe';
import { LocalizedDatePipe } from '../pipes/localized-date.pipe';
import { LocalizedPhonePipe } from '../pipes/localized-phone.pipe';

// Componentes compartidos
import { LoaderComponent } from '../components/loader/loader.component';
import { LanguageSelectorComponent } from '../components/language-selector/language-selector.component';
import { ModalComponent } from './modal/modal.component';

/**
 * SharedModule - Módulo compartido entre todos los módulos lazy-loaded
 * Contiene módulos comunes, pipes y componentes que deben estar disponibles en todos los módulos
 */
@NgModule({
  declarations: [
    // Pipes compartidos
    TranslatePipe,
    LocalizedDatePipe,
    LocalizedPhonePipe,
    // Componentes compartidos
    LoaderComponent,
    LanguageSelectorComponent
    ,
    ModalComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule
  ],
  exports: [
    // Re-exportar módulos de Angular para uso en lazy modules
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    // Re-exportar pipes para uso en lazy modules
    TranslatePipe,
    LocalizedDatePipe,
    LocalizedPhonePipe,
    // Re-exportar componentes compartidos
    LoaderComponent,
    LanguageSelectorComponent
    ,
    ModalComponent
  ]
})
export class SharedModule { }

