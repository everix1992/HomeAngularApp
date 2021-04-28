import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Title } from "@angular/platform-browser";

@Component({
  selector: 'app-advent-of-code',
  templateUrl: './advent-of-code.component.html',
  styleUrls: ['./advent-of-code.component.css']
})
export class AdventOfCodeComponent implements OnInit {
  public inputText?: string;
  public resultText: string = "";
  public selectedSolution?: string;
  public solutionNames: string[] = [];

  constructor(private http: HttpClient, private titleService: Title) {
    this.titleService.setTitle("Advent of Code Challenges" + environment.websiteTitleSuffix);
  }

  async ngOnInit() {
    const apiUrl = new URL('/api/v1/AdventOfCodeSolutions', environment.apiBaseUrl);
    const results = await this.http.get<string[]>(apiUrl.href).toPromise();

    if (results && results.length > 0) {
      this.solutionNames = results;
      this.selectedSolution = this.solutionNames[0];
    }
  }

  clearButtonClicked() {
    this.inputText = undefined;

    if (this.solutionNames && this.solutionNames.length > 0) {
      this.selectedSolution = this.solutionNames[0];
    }

    this.resultText = "";
  }

  async runButtonClicked() {
    const apiUrl = new URL('/api/v1/AdventOfCodeSolutions/GetSolution', environment.apiBaseUrl);

    // TODO: Strongly type this
    const requestBody = {
      Input: this.inputText,
      SolutionName: this.selectedSolution
    };

    const result = await this.http.post<string>(apiUrl.href, requestBody).toPromise();
    this.resultText = result;
  }

}
