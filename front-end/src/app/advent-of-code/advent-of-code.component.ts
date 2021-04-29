import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Title } from "@angular/platform-browser";
import { SolutionViewModel } from './SolutionViewModel';

@Component({
  selector: 'app-advent-of-code',
  templateUrl: './advent-of-code.component.html',
  styleUrls: ['./advent-of-code.component.css']
})
export class AdventOfCodeComponent implements OnInit {
  public currentFileUploaded: boolean = false;
  public currentInputId?: string;
  public currentTextUploaded: boolean = false;
  public inputText?: string;
  public resultText: string = "";
  public selectedFile?: File;
  public selectedSolution?: SolutionViewModel;
  public solutions: SolutionViewModel[] = [];

  private controllerUrl: string = "/api/v1/advent-of-code";

  constructor(private http: HttpClient, private titleService: Title) {
    this.titleService.setTitle("Advent of Code Challenges" + environment.websiteTitleSuffix);
  }

  async ngOnInit() {
    const apiUrl = new URL(this.controllerUrl, environment.apiBaseUrl);
    const results = await this.http.get<SolutionViewModel[]>(apiUrl.href).toPromise();

    if (results && results.length > 0) {
      this.solutions = results;
      this.selectedSolution = this.solutions[0];
    }
  }

  clearButtonClicked() {
    this.inputText = undefined;
    this.currentTextUploaded = false;
    this.selectedFile = undefined;
    this.currentFileUploaded = false;
    this.resultText = "";
    this.deleteCurrentInput();
  }

  fileInputChanged(event: any) {
    if (event?.target?.files?.length > 0) {
      this.selectedFile = event.target.files[0];
    } else {
      this.selectedFile = undefined;
    }
  }

  inputTextChanged() {
    this.currentTextUploaded = false;
  }

  async runButtonClicked() {
    const getSolutionUrl = new URL(`${this.controllerUrl}/${this.selectedSolution?.id}?inputId=${this.currentInputId}`, environment.apiBaseUrl);
    const result = await this.http.get<string>(getSolutionUrl.href).toPromise();
    this.resultText = result;
  }

  async uploadFileClicked() {
    if (this.selectedFile == undefined) {
      return;
    }

    this.deleteCurrentInput();
    const postInputUrl = new URL(`${this.controllerUrl}/input-file`, environment.apiBaseUrl);

    let formData = new FormData();
    formData.append('file', this.selectedFile as File);

    this.currentInputId = await this.http.post(postInputUrl.href, formData, { reportProgress: true, responseType: 'text'}).toPromise();
    this.currentFileUploaded = true; // TODO: Potential issue here if user changes file before the POST call returns
  }

  async uploadTextClicked() {
    this.deleteCurrentInput();
    const postInputUrl = new URL(`${this.controllerUrl}/input`, environment.apiBaseUrl);
    this.currentInputId = await this.http.post(postInputUrl.href, this.inputText, {responseType: 'text'}).toPromise();
    this.currentTextUploaded = true; // TODO: Potential issue here if user changes input text before the POST call returns
  }

  private deleteCurrentInput() {
    if (this.currentInputId != null) {
      const deleteInputUrl = new URL(`${this.controllerUrl}/input/${this.currentInputId}`, environment.apiBaseUrl);
      this.currentInputId = undefined;
      this.http.delete(deleteInputUrl.href);
    }
  }
}
